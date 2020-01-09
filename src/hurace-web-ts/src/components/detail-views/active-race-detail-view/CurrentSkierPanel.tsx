import React from 'react';
import { HeaderCard } from '../../shared/HeaderCard';
import { SkierListViewItem } from '../../list-views/skier-list-view/SkierListViewItem';
import { StartList } from '../../../models/StartList';
import {
    RowFlex,
    NoListEntryText,
    AlignRight
} from '../../../theme/CustomComponents';

export const CurrentSkierPanel: React.FC<{
    currentSkier: StartList | undefined;
}> = ({ currentSkier }) => {
    return (
        <AlignRight>
            <HeaderCard headerText="Aktueller Läufer">
                <RowFlex>
                    {currentSkier ? (
                        <SkierListViewItem skier={currentSkier.skier} />
                    ) : (
                        <NoListEntryText>
                            Kein Läufer auf der Strecke
                        </NoListEntryText>
                    )}
                </RowFlex>
            </HeaderCard>
        </AlignRight>
    );
};
