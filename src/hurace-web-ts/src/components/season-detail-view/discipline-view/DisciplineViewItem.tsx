import React from 'react';
import styled from 'styled-components';
import { DisciplineData } from '../../../models/DisciplineData';
import { RaceListViewItem } from './RaceListViewItem';
import { DefaultLink } from '../../../theme/CustomComponents';
import { HeaderCard } from '../../shared/HeaderCard';

const PanelWrapper = styled.div`
    margin-bottom: ${props => props.theme.gap};
`;

const RacesPanel = styled.div`
    display: flex;
    flex-wrap: wrap;
`;

export const DisciplineViewItem: React.FC<{
    disciplineData: DisciplineData;
    seasonId: number;
}> = ({ disciplineData, seasonId }) => {
    return (
        <PanelWrapper>
            <HeaderCard headerText={disciplineData.discipline.disciplineName}>
                <RacesPanel>
                    {disciplineData.races.map(r => (
                        <DefaultLink
                            key={r.id}
                            to={`/season/${seasonId}/race/${r.id}`}
                        >
                            <RaceListViewItem race={r} />
                        </DefaultLink>
                    ))}
                </RacesPanel>
            </HeaderCard>
        </PanelWrapper>
    );
};
