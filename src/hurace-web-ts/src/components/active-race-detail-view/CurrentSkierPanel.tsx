import React from 'react';
import { HeaderCard } from '../shared/HeaderCard';
import { SkierListViewItem } from '../skier-list-view/SkierListViewItem';
import { StartList } from '../../models/StartList';
import { RowFlex, NoListEntryText } from '../../theme/CustomComponents';
import styled from 'styled-components';

const CurrentSkierFlex = styled(RowFlex)`
    @media screen and (max-width: ${props => props.theme.mobileSize}) {
        flex-direction: column;
        align-items: center;
    }
`;

export const CurrentSkierPanel: React.FC<{
    currentSkier: StartList | undefined;
}> = ({ currentSkier }) => (
    <HeaderCard headerText="Aktueller Läufer">
        <CurrentSkierFlex>
            {currentSkier ? (
                <SkierListViewItem skier={currentSkier.skier} />
            ) : (
                <NoListEntryText>Kein Läufer auf der Strecke</NoListEntryText>
            )}
        </CurrentSkierFlex>
    </HeaderCard>
);
