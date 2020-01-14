import React from 'react';
import { Skier } from '../../models/Skier';
import styled from 'styled-components';
import { getDisciplinesForSkier } from '../../common/api';
import { useStateAsync } from '../../hooks/useStateAsync';
import { getDate } from '../../common/timeConverter';
import { HeaderCard } from '../shared/HeaderCard';
import {
    RowFlex,
    ColumnFlex,
    TextBold,
    WrapText
} from '../../theme/CustomComponents';

const StatsPanel = styled(ColumnFlex)`
    padding: ${props => props.theme.gap};
`;

const Skierdata = styled.div`
    display: grid;
    grid-template-columns: auto auto;
    column-gap: 10px;
    row-gap: 5px;
    grid-template-rows: repeat(5, auto);
`;

const Disciplines = styled(WrapText)`
    margin-top: 5px;
`;

export const SkierDetailPanel: React.FC<{ skier: Skier }> = ({ skier }) => {
    const [disciplines] = useStateAsync(getDisciplinesForSkier, skier.id);

    return (
        <HeaderCard
            contentStyles={{ padding: '0px', width: '100%', height: '100%' }}
            headerText={`${skier.firstName} ${skier.lastName}`}
        >
            <RowFlex>
                {skier.imageUrl && <img alt="Skier" src={skier.imageUrl} />}
                <StatsPanel>
                    <Skierdata>
                        <TextBold>Geschlecht:</TextBold>
                        <span>{skier.gender?.genderDescription}</span>
                        <TextBold>Geburtsdatum:</TextBold>
                        <span>{getDate(skier.dateOfBirth)}</span>
                        <TextBold>Land:</TextBold>
                        <span>{skier.country?.countryName}</span>
                        <TextBold>Status:</TextBold>
                        <span>{skier.retired ? 'Inaktiv' : 'Aktiv'} </span>
                        <TextBold>Disziplinen:</TextBold>
                    </Skierdata>
                    <Disciplines>
                        {disciplines &&
                            disciplines.map(d => d.disciplineName).toString()}
                    </Disciplines>
                </StatsPanel>
            </RowFlex>
        </HeaderCard>
    );
};
